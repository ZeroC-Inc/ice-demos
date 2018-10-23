This demo implements the simple [filesystem application][1] shown at the
end of the TypeScript client-side mapping chapter.

To run the demo, first you need to start a Manual printer server from
another language mapping (Java, C++, C#, or Python). Please refer to
the README in the server subdirectory for more information on starting
the server. If you want to get started quickly, we recommend using the
Python server.

If you're running the client in a browser, you need to start the server with
WS as it's default protocol. This is done by setting the Ice.Default.Protocol
option. If you're running the python server this would look like:

```
python server.py --Ice.Default.Protocol=ws
```

[1]: https://doc.zeroc.com/display/Ice37/Example+of+a+File+System+Client+in+Javascript
