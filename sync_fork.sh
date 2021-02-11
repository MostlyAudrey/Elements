git remote add upstream https://git@github.com/MostlyAudrey/Elements.git
git stash
git fetch upstream
git checkout main
git merge upstream/main
git pull
git push
git stash pop