# SportStore

SportStore is a modern, full-featured ASP.NET Core MVC webshop for sporting goods, built with Clean Architecture. It features a product catalog, shopping cart, order placement, authentication, and claims-based authorization. The UI is styled for a sporty, professional look and is fully responsive.

## Features

- **Product Catalog**: Browse, search, and manage sports products with images and categories.
- **Shopping Cart**: Add, remove, and update products in a session-based cart.
- **Order Placement**: Secure checkout with order confirmation and delivery details.
- **Authentication & Authorization**: Register/login with custom user fields, and claims-based access for catalog management.
- **Image Upload**: Store product images in `wwwroot` and display them in the catalog.
- **Modern UI**: Sporty, responsive design using Bootstrap, custom CSS, and FontAwesome icons.
- **Clean Architecture**: Separation of concerns across Domain, Application, Infrastructure, and WebUI projects.
- **Docker Support**: Run the app and PostgreSQL database easily with Docker Compose.

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [Docker](https://www.docker.com/) (for containerized run)

### Running with Docker (Recommended)

1. **Clone the repository:**
   ```sh
   git clone https://github.com/vanneszias/SportStore
   cd SportStore
   ```
2. **Start the app and database:**

   ```sh
   docker-compose up --build
   ```

   This will:

   - Build the app
   - Start a PostgreSQL database
   - Apply migrations and seed data automatically

3. **Visit the webshop:**
   - Open [http://localhost:8080](http://localhost:8080) in your browser.

### Running Locally (Without Docker)

1. **Set up the database:**

   - Ensure PostgreSQL is running locally (default: `localhost:5432`).
   - Update the connection string in `SportStore.WebUI/appsettings.json` if needed.

2. **Apply migrations:**

   ```sh
   dotnet ef database update --project SportStore.Infrastructure --startup-project SportStore.WebUI
   ```

3. **Run the application:**
   ```sh
   dotnet run --project SportStore.WebUI
   ```
   - The app will be available at [http://localhost:5000](http://localhost:5000) or as configured.

## Default Users

- The database is seeded with two users:
  - **Admin**: Can manage the catalog (add/edit/delete products)
  - **User**: Can browse and order
- The **Admin** user credentials can be found in the [Programs.cs](SportStore.WebUI/Program.cs)

## Project Structure

- `SportStore.Domain`: Core entities and logic
- `SportStore.Application`: Interfaces and business logic
- `SportStore.Infrastructure`: Data access, EF Core, repositories
- `SportStore.WebUI`: MVC app, views, controllers, static files

## Customization

- Update styles in `SportStore.WebUI/wwwroot/css/sportstore.css`
- Add new features or sports categories in the database seed or via the UI

## License

MIT

---

For questions or contributions, open an issue or contact me
