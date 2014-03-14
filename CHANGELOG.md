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