#! /bin/bash
set -eou pipefail


TAG=${1}
DOCKER_IMAGE="ghcr.io/therealtbs/pgkavita";
DOCKER_TAG="${TAG#v}"

function section() {
    echo '';
    echo $1;
    echo '---------------------------------------------------------';
}

cd workspace;

section 'Building App';

./build.sh;

pushd ../DataMigrationTool;

section 'Building DataMigrationTool';

./build.sh;

popd;

section 'Re-creating archives with DataMigrationTool';

for platform in linux-x64 linux-arm linux-arm64; do
    cp -r ../_output/${platform}/DataMigrationTool _output/${platform}/DataMigrationTool;
    tar -czvf $PWD/_output/kavita-${platform}.tar.gz -C _output/${platform} Kavita DataMigrationTool;
done

section 'Dockering';

ADDITIONAL_TAGS="";

if [[ -n "${2-}" ]]; then
    ADDITIONAL_TAGS="-t ${DOCKER_IMAGE}:${2}"
fi

docker buildx build -t "${DOCKER_IMAGE}:${TAG#v}" ${ADDITIONAL_TAGS} --platform linux/amd64,linux/arm/v7,linux/arm64 . --push;
