# Copyright (c) ZeroC, Inc.

import asyncio
import Ice

# Slice module VisitorCenter in Greeter.ice maps to Python module VisitorCenter.
import VisitorCenter

class Chatbot(VisitorCenter.Greeter):
    """
    Chatbot is an Ice servant that implements Slice interface Greeter.
    """

    def __init__(self, loop: asyncio.AbstractEventLoop):
        self.loop = loop

    # Implements the method greet from the Greeter class generated by the Slice compiler.
    # This variant is the asynchronous implementation.
    def greet(self, name: str, current: Ice.Current) -> str:

        async def slow_greet(name: str) -> str:
            await asyncio.sleep(1)
            print(f"Dispatching greet request {{ name = '{name}' }}")
            return f"Hello, {name}!"

        # Run the slow_greet coroutine in the asyncio event loop. We use run_coroutine_threadsafe because this is
        # executed by a thread from the Ice server thread pool.
        return asyncio.run_coroutine_threadsafe(slow_greet(name), self.loop)
