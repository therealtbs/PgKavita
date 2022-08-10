#! /bin/bash
set -e

outputFolder='_output'

ProgressStart()
{
    echo "Start '$1'"
}

ProgressEnd()
{
    echo "Finish '$1'"
}

Build()
{
    ProgressStart 'Build'

    rm -rf $outputFolder

    slnFile=DataMigrationTool.csproj

    if [[ -z "$RID" ]];
    then
        dotnet msbuild -restore $slnFile -p:Configuration=Release -p:Platform="Any CPU"
    else
        dotnet msbuild -restore $slnFile -p:Configuration=Release -p:Platform="Any CPU" -p:RuntimeIdentifiers=$RID
    fi

    ProgressEnd 'Build'
}


Package()
{
    local framework="$1"
    local runtime="$2"
    local lOutputFolder=../_output/"$runtime"/DataMigrationTool

    ProgressStart "Creating $runtime Package for $framework"

    # TODO: Use no-restore? Because Build should have already done it for us
    echo "Building"
    echo dotnet publish -c Release --self-contained --runtime $runtime -o "$lOutputFolder" --framework $framework -p:RuntimeIdentifier=$runtime
    dotnet publish -c Release --self-contained --runtime $runtime -o "$lOutputFolder" --framework $framework -p:RuntimeIdentifier=$runtime

    echo "Creating tar"
    cd ../_output/"$runtime"/
    tar -czvf ../dmt-$runtime.tar.gz DataMigrationTool


    ProgressEnd "Creating $runtime Package for $framework"
}


RID="$1"

Build

dir=$PWD

if [[ -z "$RID" ]];
then
    Package "net6.0" "linux-x64"
    cd "$dir"
    Package "net6.0" "linux-arm"
    cd "$dir"
    Package "net6.0" "linux-arm64"
    cd "$dir"
else
    Package "net6.0" "$RID"
    cd "$dir"
fi
