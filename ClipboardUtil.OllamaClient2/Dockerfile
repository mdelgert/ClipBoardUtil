# Use the official .NET SDK image to build and publish the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Set the working directory inside the container
WORKDIR /app

# Copy the CSPROJ file and restore any dependencies
COPY *.csproj ./
RUN dotnet restore

# Copy the rest of the application code
COPY . ./

# Build the application
RUN dotnet publish -c Release -o out

# Use the official .NET runtime image to run the app
FROM mcr.microsoft.com/dotnet/runtime:8.0 AS runtime

# Set the working directory inside the container
WORKDIR /app

# Copy the build output from the previous stage
COPY --from=build /app/out ./

# Set environment variable to indicate the app is running inside Docker
ENV RUNNING_IN_DOCKER=true

# Set the entry point to run the application
ENTRYPOINT ["dotnet", "ClipboardUtil.OllamaClient2.dll"]
