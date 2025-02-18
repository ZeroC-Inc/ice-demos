// Copyright (c) ZeroC, Inc.

import { Ice } from "@zeroc/ice";
import { VisitorCenter } from "./Greeter.js";

// Create an Ice communicator to initialize the Ice runtime. The communicator is disposed before the program exits.
await using communicator = Ice.initialize(process.argv);

// GreeterPrx is a class generated by the Slice compiler. We create a proxy from a communicator and a "stringified
// proxy" with the address of the target object.
// If you run the server on a different computer, replace localhost in the string below with the server's hostname
// or IP address.
const greeter = new VisitorCenter.GreeterPrx(communicator, "greeter:tcp -h localhost -p 4061");

// Send a request to the remote object and get the response.
const greeting = await greeter.greet(process.env.USER || process.env.USERNAME || "masked user");

console.log(greeting);
