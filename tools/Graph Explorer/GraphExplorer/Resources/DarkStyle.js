function graphSurfaceDefaultBackgroundColor() {
    return 'black';
}

function graphSurfaceDefaultForegroundColor() {
    return 'gray';
}

function createGraphNode(node) {
    if (node.labels[0] == 'Class') {
        var image = "Resources/Class_16x.svg";
        return { id: node.id, value: node.properties.WMC, label: node.properties.Name, title: node.properties.$indegree, color: "red", shape: 'image', 'image': image }
    }
    else if (node.labels[0] == 'Interface') {
        var image = "Resources/Interface_16x.svg";
        return { id: node.id, label: node.properties.Name, title: node.properties.Name, shape: 'image', 'image': image }
    }
    else if (node.labels[0] == 'Method') {
        // Find the correct image to use, depending on the method visibility
        var image = "Method_16x.svg";
        if (node.properties.Visibility == 'private')
            image = "MethodPrivate_16x.svg";
        else if (node.properties.Visibility == 'protected')
            image = "MethodProtect_16x.svg";
        else if (node.properties.Visibility == 'internal')
            image = "MethodFriend_16x.svg";
        else if (node.properties.Visibility == 'final')
            image = "MethodSealed_16x.svg";
        return { id: node.id, value: node.properties.LOC, label: node.properties.Name, title: 'Method ' + node.properties.Name, shape: 'image', image: 'Resources/' + image, color: "green" }
    }
    return { id: node.id, label: node.properties.Name, title: node.properties.Name };
}

function createGraphEdge(edge) {
    if (edge.type == 'CALLS') {
        return { id: edge.id, from: edge.from, to: edge.to, value: edge.properties.Count, label: edge.type, title: 'Calls: ' + edge.properties.Count, color: {color: 'darkgrey'}};
    }
    else {
        return { id: edge.id, from: edge.from, to: edge.to, label: edge.type };
    }
}

function options() {
    return {
        interaction: { hover: true, selectConnectedEdges: false },
        manipulation: {
            enabled: false, // true enables adding nodes to the graph
        },
        nodes: {
            size: 10, // For nodes that do not have specific size or where no value attribute is provided
            font: { strokeWidth: 0, color: "white", bold: false }, // This is the amount of space around the text in nodes or edges.
            shape: 'dot',
            scaling: {
                min: 10, max: 30,
                label: { // Make sure font size is in this range.
                    min: 8, max: 24
                }
            }
        },
        edges: {
            arrows: "to",
            shadow: false,
            smooth: true,
            font: { color: 'white', strokeWidth: 0, opacity: 1.0},
            scaling: {
                min: 2, 'max': 12, 'label': { 'enabled': true, 'min': 9, 'max': 14}
            }
        }
    }
}
