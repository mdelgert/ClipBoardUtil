# Links
https://github.com/marketplace/models/azureml-meta/Meta-Llama-3-1-405B-Instruct
https://nodejs.org/en/download/package-manager/all#debian-and-ubuntu-based-linux-distributions
https://github.com/nodesource/distributions?tab=readme-ov-file#ubuntu-versions

# Node setup option 1 (v12.22.9)
```bash
sudo apt install nodejs
node --version
```

# Node setup option 2
```bash
sudo apt remove nodejs
sudo apt-get install -y curl
curl -fsSL https://deb.nodesource.com/setup_22.x -o nodesource_setup.sh
sudo -E bash nodesource_setup.sh
sudo apt-get install -y nodejs
node -v
```

# Setup github pat
https://github.com/settings/tokens
```bash
nano ~/.bashrc
export GITHUB_TOKEN="<your-github-token-goes-here>"
source ~/.bashrc
echo $GITHUB_TOKEN
```

# Run the app
```bash
node -v
npm install
node sample1.js
```