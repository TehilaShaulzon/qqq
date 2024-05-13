# RL Authorization System

This project is an RL (Role-based Authorization) system designed to manage to-do items and users efficiently. It provides a set of endpoints to interact with both to-do items and users based on the role of the user making the request.

## Endpoints

### To-do Items

- **GET /api/todo**
  - **Role**: User/Admin
  - **Description**: Retrieve a list of all to-do items for the current user.
  - **Response Body**: List of to-do items.

- **GET /api/todo/{id}**
  - **Role**: User/Admin
  - **Description**: Retrieve a specific to-do item by its ID.
  - **Response Body**: Details of the requested to-do item.

- **POST /api/todo**
  - **Role**: User/Admin
  - **Description**: Add a new to-do item for the current user.
  - **Request Body**: Details of the new to-do item.
  - **Response Body**: Location of the newly created to-do item.

- **PUT /api/todo/{id}**
  - **Role**: User/Admin
  - **Description**: Update details of a specific to-do item.
  - **Request Body**: Updated details of the to-do item.

- **DELETE /api/todo/{id}**
  - **Role**: User/Admin
  - **Description**: Delete a specific to-do item.

### Users

- **GET /api/user**
  - **Role**: User/Admin
  - **Description**: Retrieve details of the current user.
  - **Response Body**: Details of the current user.

- **GET /api/user**
  - **Role**: Admin
  - **Description**: Retrieve a list of all users.
  - **Response Body**: List of users.

- **POST /api/user**
  - **Role**: Admin
  - **Description**: Add a new user to the system.
  - **Request Body**: Details of the new user.
  - **Response Body**: Location of the newly created user.

- **DELETE /api/user/{id}**
  - **Role**: Admin
  - **Description**: Delete a specific user from the system, along with all associated to-do items.

## Server-Side Notes

- Only administrators have the authority to add or delete users.
- Users can only manage their own to-do items and cannot access the items of other users.
- Both to-do items and users are stored in a JSON file.
- Access to to-do items and users is facilitated through an injected service with an interface, allowing for easy transition to a database-based application.
- An extension method of IServiceCollection is provided for simplified registration.
- Each request is logged to a file, capturing start date & time, controller & action names, logged-in user (if applicable), and operation duration in milliseconds.

## Client-Side Notes

- The default page displays the user’s to-do list and offers functionalities for adding, updating, and deleting items.
- In the absence of a logged-in user (expired token or no token saved in local storage), the user is redirected to a login page.
- Administrators have access to a link from the to-do list page to the users list page, and vice versa.
- A Postman button is available on the login page for convenient testing.

## Challenges

- Implement functionality to allow users to update their own details, such as name and password.
- Enable administrators to view and edit their own to-do items as regular users.
- Implement user authentication using Google accounts.

---

