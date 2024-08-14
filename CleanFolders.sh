#!/bin/bash

# Script name: CleanFolders.sh
# Description: Deletes folders named "bin" or "obj" in the current directory and all its subdirectories.

# Loop through each file and folder in the current directory and all its subdirectories
for entry in *; do
  # Check if the entry is a directory
  if [ -d "$entry" ]; then
    # Search the directory for folders named "bin" or "obj"
    for subentry in "$entry"/*; do
      if [ -d "$subentry" ]; then
        # Check if the directory name matches "bin" or "obj"
        if [ "$subentry" == "${entry}/bin" ] || [ "$subentry" == "${entry}/obj" ]; then
          # If it's a match, delete the folder
          rm -rf "$subentry"
          echo "Deleted folder: $subentry"
        fi
      fi
    done
  fi
done

exit 0