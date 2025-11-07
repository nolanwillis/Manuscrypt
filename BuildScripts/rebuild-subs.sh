set -e

cd /home/nolanwillis/Code/Manuscrypt

docker build -t subscriptionservice:latest -f Manuscrypt.SubscriptionService/Dockerfile .

kind load docker-image subscriptionservice:latest --name dev-cluster

echo "Subscription service rebuilt and loaded into kind."

echo "Restarting subscriptionservice deployment..."
kubectl rollout restart deployment/subscriptionservice