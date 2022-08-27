# streetmapping
given a data set representing roads and intersections the program finds shortest path between any two intersections using Dijkstra's algorithm.

The map data is read from a text file containing lines representing intersections or roads and each line has 4 pieces of data separated by tab.
An input line may be an intersection or road
Intersections start with “i”, followed by a unique string ID, and decimal representations of latitude and longitude.
i IntersectionID Latitude Longitude
Roads start with “r”, followed by a unique string ID, and the IDs of the two intersections it connects.
r RoadID Intersection1ID Intersection2ID
