import React from "react";
import { Button } from 'reactstrap';
import userManager from "../../utils/userManager";

class Login extends React.Component {
  onLoginButtonClick = event => {
    event.preventDefault();
    userManager.signinRedirect();
  };

  render() {
    return (
      <div>
        <Button color="primary" onClick={this.onLoginButtonClick}>Login with IDP</Button>
      </div>
    );
  }
}

export default Login;
