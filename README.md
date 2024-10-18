A group project – a console application that simulates a TFL Trip Planner using Dijkstra's Algorithm to find the shortest routes between locations. This project also involves hand-coded data structures, showcasing an in-depth understanding of algorithms and data management.

Features

- Uses Dijkstra's Algorithm to compute the shortest path for public transport routes.
- Hand-crafted implementations of core data structures, including:
    - List
    - Dictionary (using a binary tree)
    - Priority Queue (using a min-heap)
- Focuses on readability and maintainability throughout the codebase.

Technologies Used

- C#: The core language for implementing the solution.

Design Patterns

- Singleton: Ensures certain components (like the Trip Planner) have a single instance.
- Factory: Used to generate transport nodes dynamically based on route data.

SOLID Principles

- Single Responsibility Principle (SRP): Each class has one responsibility, making it easier to maintain and extend.
- Dependency Inversion Principle (DIP): Interfaces are used to abstract implementations, promoting flexibility.
