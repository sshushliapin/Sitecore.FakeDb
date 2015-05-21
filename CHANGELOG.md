0.26.1
* [FIX] #58: Versioned field value is not empty for item in `Invariant` language

0.26.0
* [NEW] #26: Include configs
* [NEW] #46: Wish: Add default field types to app.config
* [NEW] #34: Automatic template generation needs to be a little smarter
* [NEW] #52: Allow reuse of generated templates across all items, not only siblings
* [FIX] #54: Adding a duplicated item throws an exception with a helpless message
* [FIX] #55: Deserialize item fails if there is an item with the same id
* [FIX] #56: Deserializing a linked template throws an error if the '__BaseTemplates' field is null

0.25.3
* [FIX] #49: Unable to edit an **empty** item field inherited from a base template
* [FIX] #50: Update tests to xunit2

0.25.2
* [FIX] #48: Deserialization of base templates broken in latest version

0.25.1
* [FIX] #44: Fix Template.OwnFields

0.25.0
* [NEW] #40: Create a protected item
* [NEW] #41: Auto-translate should be disabled by default
* [NEW] #42: Move database commands configuration out of the App.conifg file

0.24.0
* [NEW] #36: Introduce strongly typed LinkField configuration
* [NEW] #37: Added support for shortened paths in serialized data. (thanks to @hermanussen)
* [FIX] #35: Item.DeleteChildren() throws NullReferenceException if no fake item found in DataStorage
* [FIX] #39: Encapsulate thread local datastorage management in the DataEngineCommand class
* [FIX] #38: Get rid of the DataStorage dependency in the PipelineWatcher class
* [FIX] #30: Using provided NUnit runner in JetBrains TeamCity. Partially fixed, waiting for Sitecore update

0.23.0
* [NEW] #32: Introduce IDTable provider switching
* [NEW] #33: Specify item branch id
* [FIX] #31: Use a higher level ItemManager.CopyItem() instead of ItemManager.Provider

0.22.0
* [NEW] Implemented blob streams
* [FIX] Fixed "Item Versions #28". Method ItemManager.AddFromTemplate() should create an item with a single version while ItemManager.CreateItem() just creates an item with no versions

0.21.0
* [NEW] Switching database when initializing new Db context
* [NEW] Introduced simple FakeSiteContext
* [NEW] Published Symbol Packages
* [FIX] Locked config to avoid race conditions when deals with in-memory settings or pipelines
* [FIX] Fixed error when regestering the same pipeline twice
* [FIX] #25: Ensure Shared property of a field is properly propagated when FakeDb generates a template and also ensure copying an item carries it over (along with the Type property). Thanks to [Pavel Veller](https://github.com/pveller)

0.20.0
* [NEW] Added possibility to register and call mocked pipeline processor
* [NEW] Introduced 'addDbItem' pipeline
* [FIX] Serialization pipeline configuration moved to the Serialization nuget package
* [FIX] Fixed field value loosing when creating an item via ItemManager
* [FIX] Fixed versions count updating when adding/removing versions

0.19.0
* [NEW] #21: Added DsDbItem and DsDbTemplate for using deserialized data in tests (thanks to [Robin Hermanussen](https://github.com/hermanussen))
* [NEW] #22: Add field propagation from all inherited templates and refactor the way FakeDb works with fields (thanks to [Pavel Veller](https://github.com/pveller))

0.18.1
* [FIX] Added Link Manager registration

0.18.0
* [NEW] Introduced Item Statistics
* [FIX] Share auto-generated templates for siblings only
* [FIX] Fixed item version removing
* [FIX] Fixed item languages retrieving

0.17.0
* [NEW] Implemented limited Sitecore Fast Query support

0.16.1
* [FIX] Fixed issue when template data might be lost
* [FIX] Clean CorePipelineFactory cache on db disposing
* [FIX] Fixed item children deleting

0.16.0
* [NEW] Implemented security settings inheritance. Ingroduced "__Secutiry" standard field
* [NEW] Implemented Shared fields support
* [NEW] Implemented Fake Membership Provider
* [FIX] FakeDataProvider has got thread local DataStorage to solve concurrency issues
* [FIX] Fixed item versioning issue

0.15.0
* [NEW] Implemented Fake Authorization Provider
* [NEW] Implemented Fake Role Provider
* [NEW] Implemented Fake RolesInRoles Provider
* [FIX] DataEngine commands became thread local to solve concurrency issue

0.14.0
* [NEW] Implemented context user switching
* [NEW] Reconfigured authentication provider
* [NEW] Introduced FakeUserProfile. Thanks to [Pavel Veller](https://github.com/pveller)
* [NEW] Implemented item locking

0.13.0
* [NEW] Implemented Link Database, Task Database and Media Provider switchers
* [NEW] Implemented fake text translation
* [NEW] Introduced Sitecore Query support
* [FIX] Implemented deep copy
* [FIX] Default language is set to "en-US"
* [FIX] Settings return empty string as default value
* [FIX] Fixed pipeline watcher initialization which allows to use "WhenCall" and "Then" blocks ignoring "WithArgs" part

0.12.0
* [NEW] Implemented item moving
* [NEW] Implemented item copying
* [NEW] Added Link and Task database stubs
* [FIX] Db disposing actions have been moved to "releaseFakeDb" pipeline
* [FIX] Fixed settings resetting

0.11.1
* [FIX] Fixed monitroing of multiple pipeline calls

0.11.0
* [NEW] Added the ability to work with fields by ID. Resolve names automatically for certain standard fields. Add a test showing how standard values work on the layout fields (layout deltas). Thanks to [Pavel Veller](https://github.com/pveller)
* [NEW] Added support for GetLanguages() to FakeDataProvider and make it return only one "en" language. Thanks to [Pavel Veller](https://github.com/pveller)
* [NEW] Added '/sitecore/media library' and '/sitecore/system' items as part of initial DataStorage setup. Thanks to [Pavel Veller](https://github.com/pveller)
* [NEW] Added support for field types. Thanks to [Pavel Veller](https://github.com/pveller)
* [NEW] Added basic support for templates hierarchies. No fields propagation, just BaseIDs and Template.InheritsFrom(). Thanks to [Pavel Veller](https://github.com/pveller)
* [NEW] Implemented RemoveVersionCommand
* [FIX] Fixed creating and editing items of predefined templates
* [FIX] Fixed adding first item version

0.10.4
* [FIX] Hidden setter for DbField.ID property
* [FIX] Fixed Folder template name
* [FIX] Added temporary fix which resets global templates that might be shared across unit tests

0.10.3
* [FIX] Fixed field name loosing in some scenarios
* [FIX] Fixed getting children of the content root item

0.10.2
* [FIX] Fixed template id setting via DbItem constructor

0.10.1
* [FIX] Clean up received calls for provider mocks
* [NEW] Added Folder template

0.10.0
* [NEW] Introduced NSubstitute-based mocks for authentication, authorization and bucket providers
* [FIX] db.GetItem() can return an item by id passed as a string value

0.9.1
* [FIX] Fixed database initialization exception appeared when adding two items with two similar set of multiple fields
* [FIX] Simplified standard values implementation

0.9.0
* [NEW] Implemented versioned item initialization. Implemented version adding
* [NEW] Added Standard Values support (thanks to [Pavel Veller](https://github.com/pveller))
* [NEW] Implemented fakes for System.Web.*Provider (thanks to [Pavel Veller](https://github.com/pveller))
* [FIX] Restored single instance configuration for databases

0.8.0
* [NEW] Added possibility to configure and call pipelines

0.7.1
* [FIX] Updated installation instructions
* [FIX] Updated Getting Started code samples
* [FIX] Changed default license.xml file path. Now it is set to the root of a test project

0.7.0
* [NEW] Added possibility to configure settings in memory

0.6.4
* [FIX] Fixed concurrency issues caused by a single database instance shared across tests

0.6.3
* [FIX] Fixed creating and configuring of items in a specific language

0.6.2
* [FIX] Removed inheritance from Sitecore Database class
* [FIX] Fixed item path updating after rename
* [FIX] Default item language is set to 'en'

0.6.1
* [FIX] Fixed namespace conflict in DbItem constructors

0.6.0
* [NEW] Can create an item of specific template
* [NEW] Share templates between items with similar fields
* [NEW] Db.GetItem() can now get items by id

0.5.0
* [NEW] Introduced basic item access confuguration
* [NEW] Readme with examples included into package

0.4.0
* [NEW] Configured switching authorization provider so that it can be substituted with a mock

0.3.0
* [NEW] Introduced multilingual item configuration

0.2.0
* [NEW] Added possibility to set database name in FakeDb constructor
* [NEW] Added content search confguration so that one can mock search indexes in unit tests
* [NEW] Added analytics visitor configuration so that one can set current visitor mock in unit tests
* [NEW] Added authentication provider stub so that any request to AuthenticationManager returns dummy data

0.1.0
* Initial release