set -e

cd /home/nolanwillis/Code/Manuscrypt

docker build -t manuscryptgateway:latest -f Manuscrypt.Gateway/Dockerfile .

kind load docker-image manuscryptgateway:latest --name dev-cluster

echo "Gateway service rebuilt and loaded into kind."

echo "Restarting manuscryptgateway deployment..."
kubectl rollout restart deployment/manuscryptgateway