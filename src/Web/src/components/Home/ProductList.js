import React, { Component } from "react";
import {
  Button,
  Card,
  CardText,
  CardBlock,
  CardTitle,
  CardSubtitle,
  Container,
  Row,
  Col,
  Table,
  Input
} from "reactstrap";

class ProductList extends Component {
  constructor() {
    super();
    this.state = {
      valueChange: 1
    };
  }

  onQuantityChange(value) {
    this.state.valueChange = value;
  }

  render() {
    const { ids, products, handleAddProductClick } = this.props;
    // console.log(this.props);
    return (
      <Container>
        <Row>
          {ids.map((id, index) => (
            <div key={id} className="col-sm-6 col-lg-3">
              <Card>
                <CardBlock>
                  <CardTitle>{products[id].name} </CardTitle>
                  <CardSubtitle>{products[id].model}</CardSubtitle>
                  <CardText>Price: ${products[id].price}</CardText>
                  <Row>
                    <Col xs="6">
                      <Input
                        type="textbox"
                        defaultValue={this.state.valueChange}
                        onChange={this.onQuantityChange.bind(this)}
                      />
                    </Col>
                    <Col xs="6">
                      <Button
                        color="success"
                        onClick={() =>
                          handleAddProductClick(
                            products[id],
                            this.state.valueChange
                          )}
                      >
                        Add to cart
                      </Button>
                    </Col>
                  </Row>
                </CardBlock>
              </Card>
            </div>
          ))}
        </Row>
      </Container>
    );
  }
}

export default ProductList;
