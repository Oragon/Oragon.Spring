FROM mcr.microsoft.com/dotnet/core/aspnet:3.0 AS base
WORKDIR /app
EXPOSE 80


FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS build
WORKDIR /src
COPY Oragon.Spring.Core.AspNetCoreTest/Oragon.Spring.Core.AspNetCoreTest.csproj Oragon.Spring.Core.AspNetCoreTest/
RUN dotnet restore Oragon.Spring.Core.AspNetCoreTest/Oragon.Spring.Core.AspNetCoreTest.csproj
COPY . .
WORKDIR /src/Oragon.Spring.Core.AspNetCoreTest
RUN dotnet build Oragon.Spring.Core.AspNetCoreTest.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish Oragon.Spring.Core.AspNetCoreTest.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Oragon.Spring.Core.AspNetCoreTest.dll"]
