#!/bin/bash

set -e

function usage() {
    cat <<USAGE
    Usage: $0 <options>
    Options:
        [-p|--functionapp-path] Function app path, relative to the repository root. It can be the project directory or compiled app directory.
                                Default: '.'
        [-u|--base-uri]         Base URI of the function app.
                                Default: 'http://localhost:7071/api/'
        [-e|--endpoint]         OpenAPI document endpoint.
                                Default: 'swagger.json'
        [-o|--output-path]      Output directory to store the generated OpenAPI document, relative to the repository root.
                                Default: 'generated'
        [-f|--output-filename]  Output filename for the generated OpenAPI document.
                                Default: 'swagger.json'
        [-d|--delay]            Delay in second between the function app run and document generation.
                                Default: 30
        [-h|--help]             Show this message.
USAGE

    exit 1
}

functionapp_path="."
base_uri="http://localhost:7071/api/"
endpoint="swagger.json"
output_path="generated"
output_filename="swagger.json"
delay=30

if [[ $# -eq 0 ]]; then
    functionapp_path="."
    base_uri="http://localhost:7071/api/"
    endpoint="swagger.json"
    output_path="generated"
    output_filename="swagger.json"
    delay=30
fi

while [[ "$1" != "" ]]; do
    case $1 in
    -p | --functionapp-path)
        shift
        functionapp_path=$1
        ;;

    -u | --base-uri)
        shift
        base_uri=$1
        ;;

    -e | --endpoint)
        shift
        endpoint=$1
        ;;

    -o | --output-path)
        shift
        output_path=$1
        ;;

    -f | --output-filename)
        shift
        output_filename=$1
        ;;

    -d | --delay)
        shift
        delay=$1
        ;;

    -h | --help)
        usage
        exit 1
        ;;

    *)
        usage
        exit 1
        ;;
    esac

    shift
done

current_directory=$(pwd)

cd "$CODESPACE_VSCODE_FOLDER/$functionapp_path"

# Run the function app in the background
func start --verbose false &

sleep $delay

request_uri="$(echo "$base_uri" | sed 's:/*$::')/$(echo "$endpoint" | sed 's:^/*::')"
filepath="$CODESPACE_VSCODE_FOLDER/$(echo "$output_path" | sed 's:/*$::')/$(echo "$output_filename" | sed 's:^/*::')"

if [ ! -d "$CODESPACE_VSCODE_FOLDER/$output_path" ]; then
    mkdir "$CODESPACE_VSCODE_FOLDER/$output_path"
fi

# Download the OpenAPI document
curl $request_uri > $filepath

# Stop the function app
PID=$(lsof -nP -i4TCP:7071 | grep LISTEN | awk '{print $2}')
if [[ "" !=  "$PID" ]]; then
    kill -9 $PID
fi

cd $current_directory
