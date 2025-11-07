set -e

cd /home/nolanwillis/Code/Manuscrypt

docker build -t authservice:latest -f Manuscrypt.AuthService/Dockerfile .

kind load docker-image authservice:latest --name dev-cluster

echo "Auth service rebuilt and loaded into kind."

echo "Restarting authservice deployment..."
kubectl rollout restart deployment/authservice