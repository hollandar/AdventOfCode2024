#!/bin/bash

echo "PATH: $PATH"
echo "Terraform resolved to: $(command -v terraform)"

T=$(which terraform)
TX=$("$T")
echo "TX: $TX"
