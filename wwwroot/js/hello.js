import React from 'react'

var e = React.createElement;

class HelloWorld extends React.Component {
    render() {
        return e(
            "div",
            null,
            "Hello World"
        );
    }
}

console.log("Hello World exercise:");
const containerElement = document.getElementById('content');
ReactDOM.render(e(HelloWorld), containerElement);