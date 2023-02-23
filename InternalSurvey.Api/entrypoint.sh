#!/bin/bash

set -e
run_cmd="dotnet run --server.urls http://*:80"

until dotnet ef database update -context SurveyAppContext; do
>&2 echo "SQL Server survey is starting up"
sleep 1
done

until dotnet ef database update -context ApplicationDbContext; do
>&2 echo "SQL Server user is starting up"
sleep 1
done

>&2 echo "SQL Server is up - executing command"
exec $run_cmd
