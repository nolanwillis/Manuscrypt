set -e

cd /home/nolanwillis/Code/Manuscrypt

docker build -t commentservice:latest -f Manuscrypt.CommentService/Dockerfile .

kind load docker-image commentservice:latest --name dev-cluster

echo "Comment service rebuilt and loaded into kind."

echo "Restarting commentservice deployment..."
kubectl rollout restart deployment/commentservice