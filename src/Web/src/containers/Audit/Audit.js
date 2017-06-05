import React, { Component } from "react";
import PropTypes from "prop-types";
import { bindActionCreators } from "redux";
import { connect } from "react-redux";
import * as auditActions from "../../redux/modules/audit";
import moment from "moment";
import { Container, Row, Col, Card, CardBlock, Table } from "reactstrap";

class AuditList extends Component {
  render() {
    const { ids, audits } = this.props;
    return (
      <Container>
        <Row>
          <Col xs="12">
            <div>
              <Table striped responsive hover>
                <thead className="thead-default">
                  <tr>
                    <td>#</td>
                    <td>Service name</td>
                    <td>Method name</td>
                    <td>Action</td>
                    <td>Created</td>
                    <td>&nbsp;</td>
                  </tr>
                </thead>
                <tbody>
                  {ids.map((id, index) => (
                    <tr key={id}>
                      <td>{index + 1}</td>
                      <td>{audits[id].serviceName}</td>
                      <td>{audits[id].methodName}</td>
                      <td>{audits[id].actionMessage}</td>
                      <td>
                        {moment(audits[id].created).format(
                          "dddd, MMMM Do YYYY, h:mm:ss a"
                        )}
                      </td>
                      <td />
                    </tr>
                  ))}
                </tbody>
              </Table>
            </div>
          </Col>
        </Row>
      </Container>
    );
  }
}

class Audit extends Component {
  componentDidMount() {
    this.props.getAudits();
  }

  render() {
    const { loading, byIds, audits } = this.props.auditStore;
    if (loading) {
      return <div>Loading...</div>;
    }
    return (
      <Card>
        <CardBlock>
          <br/>
          <AuditList ids={byIds} audits={audits} />
        </CardBlock>
      </Card>
    );
  }
}

/*Audit.propTypes = {
  loading: PropTypes.bool.isRequired,
  byIds: PropTypes.array.isRequired,
  audits: PropTypes.object.isRequired,
  getAudits: PropTypes.func.isRequired
};*/

function mapStateToProps(state, ownProps) {
  return {
    auditStore: state.auditStore
  };
}

export const mapDispatchToProps = dispatch =>
  bindActionCreators(auditActions, dispatch);

export default connect(mapStateToProps, mapDispatchToProps)(Audit);
