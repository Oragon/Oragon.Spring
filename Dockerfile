FROM mcr.microsoft.com/dotnet/core/sdk:3.0

RUN mkdir /app
WORKDIR /app


ADD ./Oragon.Spring.sln                                                                                 /app
ADD ./Oragon.Spring.Aop/Oragon.Spring.Aop.csproj                                                        /app/Oragon.Spring.Aop/
ADD ./Oragon.Spring.Aop.Tests/Oragon.Spring.Aop.Tests.csproj                                            /app/Oragon.Spring.Aop.Tests/
ADD ./Oragon.Spring.Core/Oragon.Spring.Core.csproj                                                      /app/Oragon.Spring.Core/
ADD ./Oragon.Spring.Core.Tests/Oragon.Spring.Core.Tests.csproj                                          /app/Oragon.Spring.Core.Tests/
ADD ./Oragon.Spring.Extensions.DependencyInjection/Oragon.Spring.Extensions.DependencyInjection.csproj  /app/Oragon.Spring.Extensions.DependencyInjection/

# RUN find .

RUN dotnet restore ./Oragon.Spring.sln

ADD ./ /app

# RUN find .

RUN dotnet build ./Oragon.Spring.sln -c Debug --verbosity n

RUN dotnet test ./Oragon.Spring.Core.Tests/Oragon.Spring.Core.Tests.csproj --configuration Debug --output ../output--core-tests
RUN dotnet test ./Oragon.Spring.Aop.Tests/Oragon.Spring.Aop.Tests.csproj --configuration Debug --output ../output-aop-tests

RUN dotnet pack ./Oragon.Spring.Core/Oragon.Spring.Core.csproj --configuration Debug  --include-source --include-symbols --output ../output-packages
RUN dotnet pack ./Oragon.Spring.Aop/Oragon.Spring.Aop.csproj   --configuration Debug  --include-source --include-symbols --output ../output-packages