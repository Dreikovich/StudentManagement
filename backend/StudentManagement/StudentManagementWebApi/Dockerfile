FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["StudentManagementWebApi/StudentManagementWebApi.csproj", "StudentManagementWebApi/"]
RUN dotnet restore "StudentManagementWebApi/StudentManagementWebApi.csproj"
COPY . .
WORKDIR "/src/StudentManagementWebApi"
RUN dotnet build "StudentManagementWebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "StudentManagementWebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "StudentManagementWebApi.dll"]
