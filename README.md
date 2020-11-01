# Roulette 

## How to run 

Please check out the Roulette API guide in the project root on guidance for the endpoints. 

Run the executable found in the Run folder in the project root to launch the service. Or you can also open up Visual Studio and run from there. 

The app is set to run on http://localhost:5000 by default (would be https in prod). 

The roulette.postman_collection.json found in the project root can be imported into Postman to test the application. You can also go to http://localhost:5000/swagger/index.html to see the swagger representation.

The collection is set up in such a way that gameId is copied over to other requests, so you don’t have to copy it yourself. 

The latest betId is also copied over to the Update and Delete bet requests, once a bet has been added. 

Create a new game, add a few bets, and play the game. Fingers crossed you win! 

Tests can be run in Visual Studio. 

## Design decisions 

Below I will explain a few design decisions made, as well as answer some of the questions from the GitHub page. 

### Authentication 

I have not added any authentication to this application. In reality, the requests would need a header containing something like a bearer token, or an oauth token with scopes/claims. Perhaps even separate user and service oauth tokens. 

### Endpoints 

I decided to add only the endpoints required to get the system working. Explanation for these endpoints can be found in the Roulette API Guide. 

These endpoints are: 

**New Game** – to create a new game. For this endpoint, I thought it was relevant to just return the game id, for use with other endpoint calls. 

**Add Bet** – to add a bet for a customer. For this endpoint, I thought it was relevant to return the full bet object created so clients have all the necessary data. 

**Update Bet** – to update a bet amount. For this endpoint, I thought it was relevant to return the full bet object updated so clients have all the necessary data. 

**Delete Bet** – to delete a bet. Only a successful 204 No Content status is returned for this endpoint. 

**Close Betting** – to close betting for a game. Only a successful 204 No Content status is returned for this endpoint. 

**Play Game** – to play the game and get the winnings. For this endpoint, it was necessary to return the game id, the winning number, the winning bets organised by customer id, the losing bets organised by customer id, and the total winnings organised by customer id. 

In reality, more endpoints are probably needed for the full version of the API ,for example GetAllBets or GetGameStatus but I did not implement these. These can easily be added in the future based on requirements. 

### Bet types 

I have only added a handful of bet types. These are Single, Split Horizontal, Split Vertical, Red, Black, Corner and Column. 

The API has been designed in such a way that other bet types can easily be added. 

To add a new bet type, the following would need to be added in the code: 

* The BetType to the BetType enum class 

* A new BetTypeBetTypeHandler class with logic of how that bet type is won 

* A new BetTypeBetTypeValidator class with logic of how to validate the position for that bet type 

* Any required numbers to Numbers.cs 

* Any required positions to Positions.cs 

* Registration for the new BetTypeBetTypeHandler and  BetTypeBetTypeValidator classes in AddBetTypeDependencies in ServiceCollectionExtensions.cs 

* Test classes for the new BetTypeBetTypeHandler and  BetTypeBetTypeValidator classes 

Once a new bet type has been implemented, this would need to be tested, and then can easily be rolled out to production. 

Feature toggles can be implemented by wrapping the appropriate mechanism around the registration of the BetTypeBetTypeHandler and BetTypeBetTypeValidator classes. 

### Storage 

I have stored games and bets in separate concurrent dictionaries in GameRepositoryStub.cs and BetRepositoryStub.cs respectively. In reality, these would be stored in a database which I have not implemented, so please don’t read too much into the implementation of these stub classes. They are there purely to get this working. 

The database would obviously need proper design, and SQL/NOSQL queries written to access what’s required. Caching could be considered to improve performance if needed. 

### Shared Services 

As advised in the GitHub page, I did not consider any services which would be shared e.g. Game History, Customer, etc 

### ETL 

Certain data may be required to be ETL’ed during the application, or from a database. Nothing like this has been setup but can easily be added in the future. 

### Logging and metrics 

I have only logged errors in this application which can be seen in ExceptionHandlerFilter.cs, where all errors are captured and handled, and appropriate responses returned. 

For the full application, it would need to be decided what other logging is required.  

The mechanism to emit these logs to an appropriate product would need to be decided too such as Splunk or AppInsights, and then alerts/dashboards can be set up based on this. 

I have also not implemented metrics for this application. Metrics should definitely be added so that monitoring can take place. I would suggest the following: 

* Request durations/response times 

* External dependency response times (e.g. db, other services) 

* Successful/failed request events 

* If any data is to be ETL’ed via event hubs/pub subs, then both response times on that and successful/failed events 

* Thread count 

* Auth failure events 

* Server stats such as CPU Usage, Memory usage, Service restarts, etc 

A mechanism would need to be added to emit these stats, and then monitoring dashboards set up in something like Grafana, along with alerts configured in the case of critical situations. 

### Testing 

I have added unit tests and integration tests. These should be configured to run automatically on any pull request and block that pull request if any fail. They should also be added to the build pipeline when creating artifacts for dev or prod environments. I also think Smoke tests should be added to the build pipeline to ensure the application is up and running. 

I unfortunately did not have time to carry out any load testing. I don’t think any stats would have been appropriate due to the fact I did not implement a database either. 

### Release Process 

To release this to production would be pretty trivial. The following would need to happen: 

* Number of environments decided to release to up until production 

* Pipeline created to automatically test and build any artifacts for each environment, as well as configure anything else needed for the environment 

* I imagine the artifact would be a microservice that would get deployed to a cloud cluster 

* If this was to be a microservice, then the API as it is may not be appropriate and may need to be wrapped around any specific microservice infrastructure 

* Once the api is ready, it can be built and deployed to a test environment, in which case it should be tested by QA, including both functional and load testing. I imagine there would be a FE UI to go with this, so testing them working together would also need to be done. 

* Once tested, it would be ready for production where it can be built and deployed to 

### Support 

This system would be easy to support due to the aforementioned metrics being emitted once that mechanism is set up. This would allow graphs to be created, and alerts set up in case of critical situations. The logging would also be useful for potentially setting up dashboards and alerts, but also for investigations when alerts are triggered/errors occur. 

A runbook would need to be created with explicit instructions on how to deal with alerts being triggered. 