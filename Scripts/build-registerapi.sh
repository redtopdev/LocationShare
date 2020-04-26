DIR=~/git/
if [ -d $DIR ]; then
  # Take action if $DIR exists. #
  echo "git folder exists"
else
  ###  Control will jump here if $DIR does NOT exists ###
  echo "Folder  ${DIR} not found. Creating the same."
  #mkdir ~/git/
fi

cd ~/git
DIR=~/git/RegisterAPI/
if [ -d $DIR ]; then
  # Take action if $DIR exists. #
  echo "Cleaning up ~/git/RegisterAPI folder before cloning."
  rm -r -f ~/git/RegisterAPI/
else
  ###  Control will jump here if $DIR does NOT exists ###
  echo "${DIR} not found."
  
fi

echo "Cloning https://github.com/redtopdev/RegisterAPI.git"
git clone https://github.com/redtopdev/RegisterAPI.git
echo "Building ~/git/RegisterAPI/RegisterAPI.sln"
dotnet build --configuration debug ~/git/RegisterAPI/RegisterAPI.sln
echo "Publishing ~/git/RegisterAPI/Service/Register.csproj to ~/engaze/api/registration/  folder."
dotnet publish --output ~/engaze/api/registration/ ~/git/RegisterAPI/Service/Register.csproj