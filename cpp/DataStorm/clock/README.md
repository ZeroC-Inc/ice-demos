# DataStorm Clock

This demo illustrates how to implement a custom encoder and decoder for the topic value type
`chrono::system_clock::time_point`.

To build the demo run:

```shell
cmake -B build -S .
cmake --build build --config Release
```

To run the demo, start the writer and specify the name of a city:

**Linux/macOS:**

```shell
./build/writer
```

**Windows:**

```shell
build\Release\writer
```

In a separate window, start the reader:

**Linux/macOS:**

```shell
./build/reader
```

**Windows:**

```shell
build\Release\reader
```

The reader will print the time sent by the writer. You can start multiple writers and readers.
