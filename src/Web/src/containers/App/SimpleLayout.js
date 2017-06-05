import React, { Component } from "react";
import { Container, Row, Col } from "reactstrap";
import Header from "../../components/Header/";
import Footer from "../../components/Footer/";

class SimpleLayout extends Component {
  render() {
    return (
      <div className="app">
        <Header />
        <div className="app-body">
          <main className="main" style={styles.home}>
            <Container>
              <Row>
                <Col xs="12">
                  {this.props.children}
                </Col>
              </Row>
            </Container>
          </main>
        </div>
        <Footer />
      </div>
    );
  }
}

const styles = {
  home: {
    marginTop: "20px"
  }
};

export default SimpleLayout;
