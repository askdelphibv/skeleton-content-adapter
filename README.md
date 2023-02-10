# Skeleton adapter for AskDelphi
## Implemented to target a folder in the local filesystem

In the skeleton data adapter (https://github.com/askdelphibv/skeleton-content-adapter) you can find a solution that implements all plumbing that's needed when creating a data adapter for AskDelphi.

In this branch we implement the ```SampleDataResourceRepository``` and ```SampleDataTopicRepository``` classes to return sensible Process/Task content from an actual data source (being some JSON files and some images on the local filesystem).

Before you can run this, make sure that you update the ```DataFolder```path in 'appsettings.json' to point to the folder that you cloned this branch into.

Make sure that beofre releasing to production you update the API keys in the appsettings.
