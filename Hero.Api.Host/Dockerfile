FROM microsoft/dotnet:2.1-aspnetcore-runtime-alpine as base
WORKDIR /app
EXPOSE 80

FROM base AS final
WORKDIR /app
COPY ./publish/backend /app
ENTRYPOINT ["dotnet", "Hero.Api.dll"]
