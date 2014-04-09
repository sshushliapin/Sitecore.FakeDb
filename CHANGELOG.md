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