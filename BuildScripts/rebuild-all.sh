set -e

cd /home/nolanwillis/Code/Manuscrypt

docker build -t authservice:latest -f Manuscrypt.AuthService/Dockerfile .
docker build -t commentservice:latest -f Manuscrypt.CommentService/Dockerfile .
docker build -t postservice:latest -f Manuscrypt.PostService/Dockerfile .
docker build -t subscriptionservice:latest -f Manuscrypt.SubscriptionService/Dockerfile .
docker build -t manuscryptgateway:latest -f Manuscrypt.Gateway/Dockerfile .

kind load docker-image authservice:latest --name dev-cluster
kind load docker-image commentservice:latest --name dev-cluster
kind load docker-image postservice:latest --name dev-cluster
kind load docker-image subscriptionservice:latest --name dev-cluster
kind load docker-image manuscryptgateway:latest --name dev-cluster

echo "All services rebuilt and loaded into kind."

echo "Restarting all deployments..."
kubectl get deployments -o name | xargs kubectl rollout restart