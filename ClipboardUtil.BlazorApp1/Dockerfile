# Use the official .NET SDK image to build and publish the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Set the working directory inside the container
WORKDIR /src

# Copy the project files to the container
COPY . .

# Restore the project dependencies
RUN dotnet restore

# Build and publish the project
RUN dotnet publish -c Release -o /app/publish

# Use the official ASP.NET runtime image for running the application
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

# Set the working directory inside the container
WORKDIR /app

# Copy the published files from the build stage to the runtime stage
COPY --from=build /app/publish .

# Expose port 8080 for the application
EXPOSE 8080

# Set the entry point for the application
ENTRYPOINT ["dotnet", "ClipboardUtil.BlazorApp1.dll"]
