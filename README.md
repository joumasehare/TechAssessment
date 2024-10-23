# TechAssessment
## Foreword
For this assessment I decided to act like I'm working on a multi layered established application called _AcmeProduct_. The nature of the product demands that multiple apps shares the same logic, so the architecture reflects this demand.

PICTURE

I only implemented the basics to illustrate my approach
- The _WebApi_ project with 1 controller.
- The _Data_ project with a basic _Unit of Work_ and _Repository_ pattern implementation.
- A _database project_ as I prefer database first.
- The _Api_ project. This houses virtually all business logic.
- A _Common_ project, for common things ya'know.
- The _Domain_ project. Houses database accurate entities and business objects.

I did the assessment like this because I was having fun.

## Running the Solution
1. Clone it
2. Open it
3. Publish the DB
4. Amend the connection string if needed
5. Run AcmeProduct.WebApi

## Settings Deep Dive
I had to make a lot of assumptions during the design phase, so I designed it in a way that I would like to use it as a consumer.

### Setting Structure
The way the config file is structured is hierarchical, but this is not the only way to express this type of data. I made the following assertions:
1. A setting always have a key
2. A setting always belongs to a group

From those assertions, I decided on additional rules:
1. A setting key may not have whitespace or a `.` in the name
2. A setting key may not share a key with a group

Using those rules a setting can be expressed as a _key value_ pair.
| Example Key | Example Value |
|-------------|---------------|
| `Host.Company.CompanyName` | `RegisteredA` |
| `Client.Web.ShowHeader` | `True` |

Structuring settings in the manner is great for optimization and ease of use as there are no recursive SQL involved. I do some preprocessing on the keys to allow efficient querying. I break up a key into 3 parts. 

**LocalKey** is the _local_ name of a setting within it's group.
**GlobalKey** is the _global unique_ key of a setting.
**Group** is basically the _GlobalKey_ minus the _LocalKey_. This is how groups exists.

Using the above examples the breakdown will be as follows:

**LocalKey** `CompanyName`

**GlobalKey** `Host.Company.CompanyName`

**Group** `Host.Company`

### Setting Storage
The settings are saved in a _single_ SQL table. I'm not concerned about the amount of data being stored as there should not be 1000's of settings.

| Column Name | Purpose |
|-------------|---------|
| SettingId | PK, Identity |
| LocalKey | Defines the local (within a group) name of the setting |
| GlobalKey | Defines the global (full and unique) key for the setting |
| Group | Defines the group that the setting belongs to |
| SettingDataType | A enum that determines how the setting is serialized/deserialized |
| Value | The serialized string representation of a value |

### Setting a Setting
Using a `ISettingsApi` a setting can be saved in 3 different ways:

1. `SetSettingValue`. This method serializes a CLR object to a string using a predefined list of Serializers. _This method should be used as you are saving settings when you are working with C# objects_.
2. `SerializeAndSetSettingValue<TValue>`. This method adds overhead as it first deserializes a string value into a CLR object and the calls `SetSettingValue`. This was done to re-use code. _This method should be used when you don't have a CLR object available such as in the case of a service call where only a Key and a String Value is passed through._
3. `SetSettingEntity<TEntity>`. This method saves a whole object based on convenience `Attributes` see the **Convenience** section below.

All of these methods will do _key processing and key validation_.

### Retrieving a Setting
Using a `ISettingsApi` a setting can be retrieved in 4 different ways:

1. `GetSettingValue<TResult>`. This method retrieves a setting for a key and deserialize it into the requested type.
2. `GetSettingValue`. This method retrieves a setting for a key and wraps it in a `Setting` object. It does not deserialize the value, and returns the string representation as saved.
3. `GetSettingsEntity<TEntity>`. This method brings back a whole object using convenience `Attributes` see the **Convenience** section below.
4. `GetSettingsInGroup`. This method returns a `List<Setting>` based on the setting group. For example querying `Host.Company` will return all settings in that group, and optionally settings in sub groups `Host.Company.*`.

### Convenience
Sometimes you want to store a group of settings as an object and retrieve it as such. Take a look at the following example keys:

```
User.FirstName
User.LastName
User.Age
```

With the use of `Attributes` you are able to create a decorated class:

```
[SettingEntity("User")]
public class User
{
    [Setting(SettingDataType.String, nameof(FirstName))]
    public string FirstName { get; set; }

    [Setting(SettingDataType.String, nameof(LastName))]
    public string LastName { get; set; }

    [Setting(SettingDataType.Int, nameof(Age))]
    public string Age { get; set; }
}
```

Using this class in the `SetSettingEntity<TEntity>` method will break up all properties into their own settings and persist it. You just call `settingsApi.SetSettingEntity(myUser)` in this example.

Taking this a bit further, settings can have child `SettingEntity` objects and again it will recursively break up the properties into settings.

```
[SettingEntity("User")]
public class User
{
    ...
    [ChildSettingEntity]
    public UserPreferences UserPreferences { get; set; }
    ...
}


[SettingEntity("User.Preferences")]
public class UserPreferences
{
    ...
}

```

To retrieve one of these objects, you simply call the `GetSettingsEntity<TEntity>` method like `settingsApi.GetSettingsEntity<User>(out var value);`. This will materialize the requested object `User` with all available properties populated. This is used in the **WebApi** to expose structured settings like `EmailConfiguration` and `HostSettings`.

### The WebApi Endpoint
The WebApi exposes 7 operations:
1. `Import`. Takes in a legacy settings file and attempts to convert and save it.
2. `SetSetting`. Used to set a setting
3. `GetSetting`. Used to get a single setting.
4. `GetHostSettings`. Gets the structured object that contains Host Settings.
5. `GetClientSettings`. Gets the structured object that contains Client Settings.
6. `GetEmailSettings`. Gets the structured object that contains Email Settings.
7. `GetSettingsInGroup`. Gets a list of settings for a given group and optionally it's sub groups.

### The Data Migration
The migration process is kicked off by the WebApi. It calls 2 methods to do the actual import
1. `ParseLegacyConfigurationFile`. This method deserializes the XML into a CLR representation of the config file. This was done using `XmlSerializer.Deserialize` into a structured set of classes. _see the namespace AcmeProduct.Api.Models.Settings.Legacy_.
2. `ImportLegacyConfigurationFile`. This method takes the deserialized objects from the previous method and serializes it into settings. It then iterates and saves settings.

## Closing thoughts
### Future improvements
Because I spent too much time already 😓

- Serialization and Deserialization is confusing and can be improved on.
- The `ImportLegacyConfigurationFile` and `SetSettingEntity` methods loops database calls. This should be a transaction with a commit after everything has been processed.
- The `RecursivelyGetSettingValues` loops database calls to retrieve settings. Consider building a list of Keys and doing only one retrieve in the `GetSettingsEntity` method.
- Consider implementing a caching mechanism like Redis that invalidates on setting saves, as the reading of settings will occur many thousands of times more than writing.
- Flesh out feature toggles into a more friendly way.
- Remove the `AutoDetectDataType` in the parsing of custom settings with a map of sorts. The risk is too great.
- Fix the multitude of bugs I'm sure are present.
- Document the WebApi and the `ISettingsApi` fully.