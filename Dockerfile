FROM mcr.microsoft.com/dotnet/aspnet:6.0-bullseye-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0-bullseye-slim AS build
WORKDIR /src
COPY ["VietnamBusiness.csproj", "./"]
RUN dotnet restore "VietnamBusiness.csproj"
COPY . .
WORKDIR "/src"
RUN dotnet build "VietnamBusiness.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "VietnamBusiness.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://+:80
ENTRYPOINT ["dotnet", "VietnamBusiness.dll"]