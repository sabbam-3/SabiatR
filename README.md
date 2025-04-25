
# SabiatR

SabiatR is a lightweight and flexible library designed for request handling and processing in .NET applications. It allows for the easy separation of request senders and handlers while providing the ability to customize and extend request handling with pipeline behaviors like logging, validation, and authorization.

With SabiatR, you can structure your application to keep it clean, maintainable, and extensible. It provides a clear separation between the request sender and handler, offering full support for pipeline behaviors that can intercept requests before and after they are handled.

🚀 Features
Request Handling: Simplifies sending and handling of commands and queries in your application.

Pipeline Behaviors: Supports adding middleware-style behaviors (such as logging, validation, and authorization) to requests at various stages of processing.

Flexible and Extendable: Easily extendable to accommodate custom logic and supports both synchronous and asynchronous operations.

Dependency Injection: Integrates seamlessly with .NET's built-in Dependency Injection container.

Type Safety: Provides strong typing for requests and handlers, supporting multiple request and response types.

⚙️ How It Works
SabiatR is built around the Mediator pattern, which helps separate the sender of a request from the handler that processes it. This allows for better decoupling of different parts of the application.

Request Handling: Requests (commands/queries) are sent through an ISender service. This service resolves the appropriate handler and processes the request.

Pipeline Behaviors: Before and after handling a request, you can define pipeline behaviors that can intercept and modify the request. For example, you could add behaviors for logging the request details, validating the request, or performing authorization checks before the request is processed.

Pipeline behaviors provide a way to add logic around the request handling process without directly modifying the handler code.

📦 Installation
SabiatR can be installed via NuGet:

bash
Copy
Edit
dotnet add package SabiatR
🛠️ Usage
Register Services
To get started, you will need to register SabiatR in your application’s service container. This is typically done in your Startup.cs or Program.cs file.

Once registered, you can easily configure your application to send requests and resolve the appropriate handlers.

Define Requests and Handlers
Requests can be defined as simple command or query objects. Each request corresponds to a handler that processes the request and provides the necessary logic for it. The handler is responsible for performing the actual work for that specific request type.

Define Pipeline Behaviors
Pipeline behaviors allow you to inject additional logic before or after a request is handled. You can define behaviors for purposes such as:

Logging: Track the incoming requests and their responses for debugging or monitoring.

Validation: Ensure that the request meets certain criteria before the handler processes it.

Authorization: Check if the request is authorized to be processed based on the user's permissions or other factors.

Behaviors can be added to the request pipeline in a modular way, and multiple behaviors can be chained together.

Sending Requests
Requests are sent through the ISender interface, which resolves the appropriate handler for the request and executes it. You can send commands, queries, or other types of requests and receive responses, depending on the nature of the request.

Handling Multiple Behaviors
SabiatR allows you to stack multiple pipeline behaviors, ensuring that each one is applied in the desired order. Each behavior can perform its logic and then pass control to the next behavior in the pipeline, or ultimately, the handler.

📚 Advanced Features
Pipeline Behavior for Commands and Queries
You can define specific behaviors for different types of requests (commands and queries). This gives you fine-grained control over how different requests are processed.

Extending SabiatR
SabiatR is designed to be easily extendable. You can create custom request types, handlers, and behaviors, or modify the way requests are processed to suit your application's needs. The library's flexibility makes it suitable for a wide variety of use cases.

📝 Contributing
We welcome contributions to SabiatR! If you have suggestions, bug fixes, or new features that would improve the library, feel free to fork the repository and submit a pull request. Contributions to the documentation are also appreciated.

📄 License
SabiatR is licensed under the MIT License. See the LICENSE file for more details.

This README provides a high-level overview of the functionality and usage of SabiatR. It emphasizes the flexibility and extensibility of the library while explaining its core features, like request handling and pipeline behaviors.