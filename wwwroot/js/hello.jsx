
var e = React.createElement;
class HelloWorld extends React.Component {
    render() {
        return <div>Hello React</div>;
    }
}


console.log("Hello World exercise:");
const containerElement = document.getElementById('content');
ReactDOM.render(e(HelloWorld), containerElement);
