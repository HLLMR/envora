# NGINX Proxy Manager (NPM) setup

Envora is designed to be reverse-proxied via **NGINX Proxy Manager** over Docker’s internal network.

## Connect NPM to Envora’s network

This repo’s compose file uses a stable network name:

- `envora_envora-network`

On your host (you already have the helper script):

- `/home/hllmr/infrastructure/connect-app-to-npm.sh envora_envora-network`

Or manually:

- `docker network connect envora_envora-network nginx-proxy-manager`

## Configure the proxy host in NPM

- **Scheme**: `http`
- **Forward Hostname / IP**: `envora-web`
- **Forward Port**: `80`
- **Websockets Support**: enabled (safe default)

## Notes

- `8080:80` is kept for direct dev access; NPM should route via the Docker network.


