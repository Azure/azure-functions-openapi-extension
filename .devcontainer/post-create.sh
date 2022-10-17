## Install additional apt packages
sudo apt-get update && \
    sudo apt-get install -y dos2unix

## Configure git
git config --global pull.rebase false
git config --global core.autocrlf input

## Enable local HTTPS for .NET
dotnet dev-certs https --trust

## CaskaydiaCove Nerd Font
# Uncomment the below to install the CaskaydiaCove Nerd Font
mkdir $HOME/.local
mkdir $HOME/.local/share
mkdir $HOME/.local/share/fonts
wget https://github.com/ryanoasis/nerd-fonts/releases/latest/download/CascadiaCode.zip
unzip CascadiaCode.zip -d $HOME/.local/share/fonts
rm CascadiaCode.zip

## AZURE CLI EXTENSIONS ##
# Uncomment the below to install Azure CLI extensions
# extensions=$(az extension list-available --query "[].name" | jq -c -r '.[]')
extensions=(account alias deploy-to-azure functionapp subscription webapp)
for extension in $extensions;
do
    az extension add --name $extension
done

## AZURE BICEP CLI ##
# Uncomment the below to install Azure Bicep CLI.
az bicep install

## AZURE FUNCTIONS CORE TOOLS ##
# Uncomment the below to install Azure Functions Core Tools. Make sure you have installed node.js
npm i -g azure-functions-core-tools@4 --unsafe-perm true

## Azurite ##
# Uncomment the below to install Azurite. Make sure you have installed node.js
npm install -g azurite

## AZURE STATIC WEB APPS CLI ##
# Uncomment the below to install Azure Static Web Apps CLI. Make sure you have installed node.js
# npm install -g @azure/static-web-apps-cli

## AZURE DEV CLI ##
# Uncomment the below to install Azure Dev CLI. Make sure you have installed Azure CLI and GitHub CLI
# curl -fsSL https://aka.ms/install-azd.sh | bash

## NGROK ##
# Uncomment the below to install ngrok.
# curl -s https://ngrok-agent.s3.amazonaws.com/ngrok.asc | \
#     sudo tee /etc/apt/trusted.gpg.d/ngrok.asc >/dev/null && \
#     echo "deb https://ngrok-agent.s3.amazonaws.com buster main" | \
#     sudo tee /etc/apt/sources.list.d/ngrok.list && \
#     sudo apt update && \
#     sudo apt install ngrok

## OH-MY-ZSH PLUGINS & THEMES (POWERLEVEL10K) ##
# Uncomment the below to install oh-my-zsh plugins and themes (powerlevel10k) without dotfiles integration
# git clone https://github.com/zsh-users/zsh-completions.git $HOME/.oh-my-zsh/custom/plugins/zsh-completions
# git clone https://github.com/zsh-users/zsh-syntax-highlighting.git $HOME/.oh-my-zsh/custom/plugins/zsh-syntax-highlighting
# git clone https://github.com/zsh-users/zsh-autosuggestions.git $HOME/.oh-my-zsh/custom/plugins/zsh-autosuggestions

# git clone https://github.com/romkatv/powerlevel10k.git $HOME/.oh-my-zsh/custom/themes/powerlevel10k --depth=1
# ln -s $HOME/.oh-my-zsh/custom/themes/powerlevel10k/powerlevel10k.zsh-theme $HOME/.oh-my-zsh/custom/themes/powerlevel10k.zsh-theme

## OH-MY-ZSH - POWERLEVEL10K SETTINGS ##
# Uncomment the below to update the oh-my-zsh settings without dotfiles integration
# curl https://raw.githubusercontent.com/justinyoo/devcontainers-dotnet/main/oh-my-zsh/.p10k-with-clock.zsh > $HOME/.p10k-with-clock.zsh
# curl https://raw.githubusercontent.com/justinyoo/devcontainers-dotnet/main/oh-my-zsh/.p10k-without-clock.zsh > $HOME/.p10k-without-clock.zsh
# curl https://raw.githubusercontent.com/justinyoo/devcontainers-dotnet/main/oh-my-zsh/switch-p10k-clock.sh > $HOME/switch-p10k-clock.sh
# chmod +x ~/switch-p10k-clock.sh

# cp $HOME/.p10k-with-clock.zsh $HOME/.p10k.zsh
# cp $HOME/.zshrc $HOME/.zshrc.bak

# echo "$(cat $HOME/.zshrc)" | awk '{gsub(/ZSH_THEME=\"codespaces\"/, "ZSH_THEME=\"powerlevel10k\"")}1' > $HOME/.zshrc.replaced && mv $HOME/.zshrc.replaced $HOME/.zshrc
# echo "$(cat $HOME/.zshrc)" | awk '{gsub(/plugins=\(git\)/, "plugins=(git zsh-completions zsh-syntax-highlighting zsh-autosuggestions)")}1' > $HOME/.zshrc.replaced && mv $HOME/.zshrc.replaced $HOME/.zshrc
# echo "
# # To customize prompt, run 'p10k configure' or edit ~/.p10k.zsh.
# [[ ! -f ~/.p10k.zsh ]] || source ~/.p10k.zsh
# " >> $HOME/.zshrc

## OH-MY-POSH ##
# Uncomment the below to install oh-my-posh
sudo wget https://github.com/JanDeDobbeleer/oh-my-posh/releases/latest/download/posh-linux-amd64 -O /usr/local/bin/oh-my-posh
sudo chmod +x /usr/local/bin/oh-my-posh

## OH-MY-POSH - POWERLEVEL10K SETTINGS ##
# Uncomment the below to update the oh-my-posh settings without dotfiles integration
# curl https://raw.githubusercontent.com/justinyoo/devcontainers-dotnet/main/oh-my-posh/p10k-with-clock.omp.json > $HOME/p10k-with-clock.omp.json
# curl https://raw.githubusercontent.com/justinyoo/devcontainers-dotnet/main/oh-my-posh/p10k-without-clock.omp.json > $HOME/p10k-without-clock.omp.json
# curl https://raw.githubusercontent.com/justinyoo/devcontainers-dotnet/main/oh-my-posh/switch-p10k-clock.ps1 > $HOME/switch-p10k-clock.ps1

# mkdir $HOME/.config/powershell
# curl https://raw.githubusercontent.com/justinyoo/devcontainers-dotnet/main/oh-my-posh/Microsoft.PowerShell_profile.ps1 > $HOME/.config/powershell/Microsoft.PowerShell_profile.ps1
# curl https://raw.githubusercontent.com/justinyoo/devcontainers-dotnet/main/oh-my-posh/Microsoft.PowerShell_profile.ps1 > $HOME/.config/powershell/Microsoft.VSCode_profile.ps1

# cp $HOME/p10k-with-clock.omp.json $HOME/p10k.omp.json

## Azure Functions - local.settings.json ##
# Uncomment the below to install local.settings.json file build without dotfiles integration
# curl https://raw.githubusercontent.com/justinyoo/devcontainers-dotnet/main/azure-functions/Build-LocalSettingsJson.ps1 > $HOME/Build-LocalSettingsJson.ps1
