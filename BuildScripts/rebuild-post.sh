set -e

cd /home/nolanwillis/Code/Manuscrypt

docker build -t postservice:latest -f Manuscrypt.PostService/Dockerfile .

kind load docker-image postservice:latest --name dev-cluster

echo "Post service rebuilt and loaded into kind."

echo "Restarting postservice deployment..."
kubectl rollout restart deployment/postservice