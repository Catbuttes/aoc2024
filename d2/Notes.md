# Notes

Dammit. I was _really_ hoping not to brute force this one. I was trying to just rescan just the levels before and after the current readings to avoid iterating through to find if I could remove a single reading and have it work. Unfortunately I found an edge case that meant I would have to backtrack much further than I was happy with using the technique I was trying to use.

So yeah, this solution is just a bog standard brute force. And an overbuilt one at that.