This demo illustrates how to access a server's [Properties facet][1]
in order to retrieve and modify its configuration properties. This
demo also shows how the server can receive notifications whenever its
properties are changed.

To run the demo, first start the server:
```
dotnet run --project server\server.csproj
```
In a separate window, start the client:
```
dotnet run --project client\client.csproj
```

[1]: https://doc.zeroc.com/ice/4.0/administration-and-diagnostics/administrative-facility/the-properties-facet
