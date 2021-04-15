/**
 * @name SQL query built from user-controlled sources
 * @description Building a SQL query from user-controlled sources is vulnerable to insertion of
 *              malicious SQL code by the user.
 * @kind path-problem
 * @problem.severity error
 * @precision high
 * @id cs/sql-injection
 * @tags security
 *       external/cwe/cwe-089
 */

import csharp
import semmle.code.csharp.security.dataflow.SqlInjection::SqlInjection
import semmle.code.csharp.dataflow.DataFlow::DataFlow::PathGraph

string getSourceType(DataFlow::Node node) {
  result = node.(RemoteFlowSource).getSourceType()
  or
  result = node.(LocalFlowSource).getSourceType()
}

class DapperQueryMethod extends Method {
  int sqlParamIdx;

  DapperQueryMethod() {
    exists(Method m | m.getQualifiedName().regexpMatch("Dapper\\.SqlMapper\\.(Query|Excecute).*") |
      this.overridesOrImplementsOrEquals(m) and sqlParamIdx = 1
    )
  }

  Parameter sqlParam() { result = this.getParameter(sqlParamIdx) }
}

class DapperSink extends Sink {
  DapperSink() {
    exists(DapperQueryMethod dqm |
      this.asExpr() = dqm.getACall().getArgumentForParameter(dqm.sqlParam())
    )
  }
}

class EnvVarSource extends LocalFlowSource {
  EnvVarSource() {
    exists(Method getEnv | getEnv.getQualifiedName() = "System.Environment.GetEnvironmentVariable" |
      this.asExpr() = getEnv.getACall()
    )
  }

  override string getSourceType() { result = "Environment Variable" }
}

from TaintTrackingConfiguration c, DataFlow::PathNode source, DataFlow::PathNode sink
where c.hasFlowPath(source, sink)
select sink.getNode(), source, sink, "Query might include code from $@.", source,
  ("this " + getSourceType(source.getNode()))
