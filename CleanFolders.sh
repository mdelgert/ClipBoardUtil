#!/bin/bash

# Script name: CleanFolders.sh
# Description: Deletes folders named "bin" or "obj" in the current directory and all its subdirectories.

# Find and delete directories named "bin" or "obj" recursively
find . -type d \( -name "bin" -o -name "obj" \) -exec rm -rf {} + -print

exit 0