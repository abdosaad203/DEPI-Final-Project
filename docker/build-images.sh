#!/usr/bin/env bash
set -euo pipefail

ROOT="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
TAG="${1:-latest}"
DOCKERFILE="${ROOT}/docker/Dockerfile"

declare -A SERVICES=(
  ["eshop/identity-api"]="src/Identity.API/Identity.API.csproj|Identity.API"
  ["eshop/basket-api"]="src/Basket.API/Basket.API.csproj|Basket.API"
  ["eshop/catalog-api"]="src/Catalog.API/Catalog.API.csproj|Catalog.API"
  ["eshop/ordering-api"]="src/Ordering.API/Ordering.API.csproj|Ordering.API"
  ["eshop/webhooks-api"]="src/Webhooks.API/Webhooks.API.csproj|Webhooks.API"
  ["eshop/order-processor"]="src/OrderProcessor/OrderProcessor.csproj|OrderProcessor"
  ["eshop/payment-processor"]="src/PaymentProcessor/PaymentProcessor.csproj|PaymentProcessor"
  ["eshop/webapp"]="src/WebApp/WebApp.csproj|WebApp"
  ["eshop/webhooksclient"]="src/WebhookClient/WebhookClient.csproj|WebhookClient"
)

for image in "${!SERVICES[@]}"; do
  IFS='|' read -r project assembly <<< "${SERVICES[$image]}"
  echo "Building ${image}:${TAG} ..."
  docker build -f "${DOCKERFILE}" \
    --build-arg PROJECT_PATH="${project}" \
    --build-arg ASSEMBLY_NAME="${assembly}" \
    -t "${image}:${TAG}" \
    "${ROOT}"
done

echo "Done. Images tagged with :${TAG}"
