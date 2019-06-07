FROM mcr.microsoft.com/dotnet/core/sdk:3.0
ARG source
WORKDIR /app
COPY ${source:-obj/Docker/publish} .
ENTRYPOINT ["dotnet", "Oragon.Spring.Core.ConsoleTest.dll"]
