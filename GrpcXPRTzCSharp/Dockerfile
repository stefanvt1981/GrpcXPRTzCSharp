#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["GrpcXPRTzCSharp/GrpcXPRTzCSharp.Server.csproj", "GrpcXPRTzCSharp/"]
COPY ["Messages/Messages.csproj", "Messages/"]
COPY ["GrpcXPRTzCSharp.Repository/GrpcXPRTzCSharp.Repository.csproj", "GrpcXPRTzCSharp.Repository/"]
RUN dotnet restore "GrpcXPRTzCSharp/GrpcXPRTzCSharp.Server.csproj"
COPY . .
WORKDIR "/src/GrpcXPRTzCSharp"
RUN dotnet build "GrpcXPRTzCSharp.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GrpcXPRTzCSharp.Server.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GrpcXPRTzCSharp.Server.dll"]